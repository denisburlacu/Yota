namespace Yota.Core
{
    public interface IYotaEntity<TYota, TEnum> : IBaseYota where TYota : IBaseYota
    {
    }
}