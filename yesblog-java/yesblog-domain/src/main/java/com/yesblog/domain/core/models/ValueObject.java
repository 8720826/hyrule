package com.yesblog.domain.core.models;

import java.io.Serializable;

/**
 * Base class for Value Objects
 */
public abstract class ValueObject<T> implements Serializable {
    
    private final T value;
    private final Integer id;
    
    protected ValueObject(T value, Integer id) {
        this.value = value;
        this.id = id;
    }
    
    public T getValue() {
        return value;
    }
    
    public Integer getId() {
        return id;
    }
    
    @Override
    public boolean equals(Object obj) {
        if (this == obj) return true;
        if (obj == null || getClass() != obj.getClass()) return false;
        ValueObject<?> that = (ValueObject<?>) obj;
        return id != null && id.equals(that.id);
    }
    
    @Override
    public int hashCode() {
        return id != null ? id.hashCode() : 0;
    }
}
