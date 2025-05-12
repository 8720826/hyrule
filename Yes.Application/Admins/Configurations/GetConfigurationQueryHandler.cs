namespace Yes.Application.Admins.Configurations
{

    public record GetConfigurationQuery() :  IRequest<GetConfigurationQueryResponse>;

    public record GetConfigurationQueryResponse(
        string Name,
        string Url,
        string Logo,
        string Keywords,
        string Description,
        string BeiAn,
        string Script,
        int PageSizeOfHomepage,
        int PageSizeOfListpage,
        string ArticleRoute,
        string PageRoute,
        string CategoryRoute,
        string SearchRoute, 
        string TagRoute,
        int TokenLifetimeMinutes,
        StorageSettingsResponse Storage
    );

    public record StorageSettingsResponse(string StorageType, QiniuSettingsResponse Qiniu);

    public record QiniuSettingsResponse(string AccessKey, string MaskedAccessKey, string SecretKey, string MaskedSecretKey, string Domain, string Bucket);

    public class GetConfigurationQueryHandler(IOptionsMonitor<BlogSettings> options, IMapper mapper) : IRequestHandler<GetConfigurationQuery, GetConfigurationQueryResponse>
    {
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IMapper _mapper = mapper;
        public async Task<GetConfigurationQueryResponse> Handle(GetConfigurationQuery request, CancellationToken cancellationToken)
        {

            var response = _mapper.Map<GetConfigurationQueryResponse>(_settings);

            return response;
        }
    }

    internal class GetConfigurationMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<BlogSettings, GetConfigurationQueryResponse>()
                .Map(dest => dest.Storage.Qiniu.MaskedAccessKey, src => string.IsNullOrEmpty(src.Storage.Qiniu.AccessKey) ? "" : "****************")
                .Map(dest => dest.Storage.Qiniu.MaskedSecretKey, src => string.IsNullOrEmpty(src.Storage.Qiniu.SecretKey) ? "" : "****************")
                .Map(dest => dest.Storage.Qiniu.AccessKey, src => "")
                .Map(dest => dest.Storage.Qiniu.SecretKey, src => "");
        }
    }


}
