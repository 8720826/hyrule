﻿namespace Yes.Infrastructure.ViewEngine
{
    // Delegate to a function that can validate a single string from a collection
    internal delegate void ValidateStringCallback(string key, string value);

    [Serializable()]
    internal class HttpValueCollection : NameValueCollection
    {
        // for implementing granular request validation
        [NonSerialized]
        private ValidateStringCallback _validationCallback;
        [NonSerialized]
        private HashSet<string> _keysAwaitingValidation;

        internal HttpValueCollection() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        // This copy constructor is used by the granular request validation feature. Since these collections are immutable
        // once created, it's ok for us to have two collections containing the same data.
        internal HttpValueCollection(HttpValueCollection col)
            : base(StringComparer.OrdinalIgnoreCase)
        {

            // We explicitly don't copy validation-related fields, as we want the copy to "reset" validation state. But we
            // do need to go against the underlying NameObjectCollectionBase directly while copying so as to avoid triggering
            // validation.
            for (var i = 0; i < col.Count; i++)
            {
                ThrowIfMaxHttpCollectionKeysExceeded();
                var key = col.BaseGetKey(i);
                var value = col.BaseGet(i);
                BaseAdd(key, value);
            }

            IsReadOnly = col.IsReadOnly;
        }

        internal HttpValueCollection(string str, Encoding encoding) : this(str, true, true, encoding) { }

        internal HttpValueCollection(string str, bool readOnly, bool urlencoded, Encoding encoding) : base(StringComparer.OrdinalIgnoreCase)
        {
            if (!string.IsNullOrEmpty(str))
                FillFromString(str, urlencoded, encoding);

            IsReadOnly = readOnly;
        }

        internal HttpValueCollection(int capacity) : base(capacity, StringComparer.OrdinalIgnoreCase)
        {
        }

        protected HttpValueCollection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /* 
         * We added granular request validation in ASP.NET 4.5 to provide a better request validation story for our developers.
         * Instead of validating the entire collection ahead of time, we'll only validate entries that are actually looked at.
         */

        internal void EnableGranularValidation(ValidateStringCallback validationCallback)
        {
            // Iterate over all the keys, adding each to the set containing the keys awaiting validation.
            // Unlike dictionaries, HashSet<T> can contain null keys, so don't need to special-case them.
            _keysAwaitingValidation = new HashSet<string>(Keys.Cast<string>().Where(KeyIsCandidateForValidation), StringComparer.OrdinalIgnoreCase);
            _validationCallback = validationCallback;

            // This forces CopyTo and other methods that cache entries to flush their caches, ensuring
            // that all values go through validation again.
            InvalidateCachedArrays();
        }

        internal const string SystemPostFieldPrefix = "__";

        internal static bool KeyIsCandidateForValidation(string key)
        {
            // Skip all our internal fields, since they don't need to be checked (VSWhidbey 275811)
            // 

            if (key != null && key.StartsWith(SystemPostFieldPrefix, StringComparison.Ordinal))
            {
                return false;
            }

            return true;
        }

        private void EnsureKeyValidated(string key)
        {
            if (_keysAwaitingValidation == null)
            {
                // If dynamic validation hasn't been enabled, no-op.
                return;
            }

            if (!_keysAwaitingValidation.Contains(key))
            {
                // If this key has already been validated (or is excluded), no-op.
                return;
            }

            // If validation fails, the callback will throw an exception. If validation succeeds,
            // we can remove it from the candidates list. Two notes:
            // - Use base.Get instead of this.Get so as not to enter infinite recursion.
            // - Eager validation skips null/empty values, so we should, also.
            var value = base.Get(key);
            if (!string.IsNullOrEmpty(value))
            {
                _validationCallback(key, value);
            }
            _keysAwaitingValidation.Remove(key);
        }

        public override string Get(int index)
        {
            // Need the key so that we can pass it through validation.
            var key = GetKey(index);
            EnsureKeyValidated(key);

            return base.Get(index);
        }

        public override string Get(string name)
        {
            EnsureKeyValidated(name);
            return base.Get(name);
        }

        public override string[] GetValues(int index)
        {
            // Need the key so that we can pass it through validation.
            var key = GetKey(index);
            EnsureKeyValidated(key);

            return base.GetValues(index);
        }

        public override string[] GetValues(string name)
        {
            EnsureKeyValidated(name);
            return base.GetValues(name);
        }

        /*
         * END REQUEST VALIDATION
         */

        internal void MakeReadOnly()
        {
            IsReadOnly = true;
        }

        internal void MakeReadWrite()
        {
            IsReadOnly = false;
        }

        internal void FillFromString(string s)
        {
            FillFromString(s, false, null);
        }

        internal void FillFromString(string s, bool urlencoded, Encoding encoding)
        {
            var l = s != null ? s.Length : 0;
            var i = 0;

            while (i < l)
            {
                // find next & while noting first = on the way (and if there are more)

                ThrowIfMaxHttpCollectionKeysExceeded();

                var si = i;
                var ti = -1;

                while (i < l)
                {
                    var ch = s[i];

                    if (ch == '=')
                    {
                        if (ti < 0)
                            ti = i;
                    }
                    else if (ch == '&')
                    {
                        break;
                    }

                    i++;
                }

                // extract the name / value pair

                string name = null;
                string value = null;

                if (ti >= 0)
                {
                    name = s.Substring(si, ti - si);
                    value = s.Substring(ti + 1, i - ti - 1);
                }
                else
                {
                    value = s.Substring(si, i - si);
                }

                // add name / value pair to the collection

                if (urlencoded)
                    base.Add(
                       HttpUtility.UrlDecode(name, encoding),
                       HttpUtility.UrlDecode(value, encoding));
                else
                    base.Add(name, value);

                // trailing '&'

                if (i == l - 1 && s[i] == '&')
                    base.Add(null, string.Empty);

                i++;
            }
        }

        internal void FillFromEncodedBytes(byte[] bytes, Encoding encoding)
        {
            var l = bytes != null ? bytes.Length : 0;
            var i = 0;

            while (i < l)
            {
                // find next & while noting first = on the way (and if there are more)

                ThrowIfMaxHttpCollectionKeysExceeded();

                var si = i;
                var ti = -1;

                while (i < l)
                {
                    var b = bytes[i];

                    if (b == '=')
                    {
                        if (ti < 0)
                            ti = i;
                    }
                    else if (b == '&')
                    {
                        break;
                    }

                    i++;
                }

                // extract the name / value pair

                string name, value;

                if (ti >= 0)
                {
                    name = HttpUtility.UrlDecode(bytes, si, ti - si, encoding);
                    value = HttpUtility.UrlDecode(bytes, ti + 1, i - ti - 1, encoding);
                }
                else
                {
                    name = null;
                    value = HttpUtility.UrlDecode(bytes, si, i - si, encoding);
                }

                // add name / value pair to the collection

                base.Add(name, value);

                // trailing '&'

                if (i == l - 1 && bytes[i] == '&')
                    base.Add(null, string.Empty);

                i++;
            }
        }

        //internal void Add(HttpCookieCollection c)
        //{
        //    int n = c.Count;

        //    for (int i = 0; i < n; i++)
        //    {
        //        ThrowIfMaxHttpCollectionKeysExceeded();
        //        HttpCookie cookie = c.Get(i);
        //        base.Add(cookie.Name, cookie.Value);
        //    }
        //}

        private const int DefaultMaxHttpCollectionKeys = int.MaxValue;

        // MSRC 12038: limit the maximum number of items that can be added to the collection,
        // as a large number of items potentially can result in too many hash collisions that may cause DoS
        internal void ThrowIfMaxHttpCollectionKeysExceeded()
        {
            if (base.Count >= DefaultMaxHttpCollectionKeys)
            {
                throw new InvalidOperationException("MaxHttpCollectionKeys");
            }
        }

        internal void Reset()
        {
            base.Clear();
        }

        public override string ToString()
        {
            return ToString(true);
        }

        internal virtual string ToString(bool urlencoded)
        {
            return ToString(urlencoded, null);
        }

        internal const string ViewStateFieldPrefixId = SystemPostFieldPrefix + "VIEWSTATE";

        internal virtual string ToString(bool urlencoded, IDictionary excludeKeys)
        {
            var n = Count;
            if (n == 0)
                return string.Empty;

            var s = new StringBuilder();
            string key, keyPrefix, item;
            var ignoreViewStateKeys = excludeKeys != null && excludeKeys[ViewStateFieldPrefixId] != null;

            for (var i = 0; i < n; i++)
            {
                key = GetKey(i);

                // Review: improve this... Special case hack for __VIEWSTATE#
                if (ignoreViewStateKeys && key != null && key.StartsWith(ViewStateFieldPrefixId, StringComparison.Ordinal)) continue;
                if (excludeKeys != null && key != null && excludeKeys[key] != null)
                    continue;
                if (urlencoded)
                    key = UrlEncodeForToString(key);
                keyPrefix = key != null ? key + "=" : string.Empty;

                var values = GetValues(i);

                if (s.Length > 0)
                    s.Append('&');

                if (values == null || values.Length == 0)
                {
                    s.Append(keyPrefix);
                }
                else if (values.Length == 1)
                {
                    s.Append(keyPrefix);
                    item = values[0];
                    if (urlencoded)
                        item = UrlEncodeForToString(item);
                    s.Append(item);
                }
                else
                {
                    for (var j = 0; j < values.Length; j++)
                    {
                        if (j > 0)
                            s.Append('&');
                        s.Append(keyPrefix);
                        item = values[j];
                        if (urlencoded)
                            item = UrlEncodeForToString(item);
                        s.Append(item);
                    }
                }
            }

            return s.ToString();
        }

        // true - use UTF8 encoding for things like <form action>, which works with modern browsers
        private static bool _dontUsePercentUUrlEncoding = true;

        // HttpValueCollection used to call UrlEncodeUnicode in its ToString method, so we should continue to
        // do so for back-compat. The result of ToString is not used to make a security decision, so this
        // code path is "safe".
        internal static string UrlEncodeForToString(string input)
        {
            if (_dontUsePercentUUrlEncoding)
            {
                // DevDiv #762975: <form action> and other similar URLs are mangled since we use non-standard %uXXXX encoding.
                // We need to use standard UTF8 encoding for modern browsers to understand the URLs.
                return HttpUtility.UrlEncode(input);
            }
            else
            {
#pragma warning disable 618 // [Obsolete]
                return HttpUtility.UrlEncodeUnicode(input);
#pragma warning restore 618
            }
        }
    }
}
