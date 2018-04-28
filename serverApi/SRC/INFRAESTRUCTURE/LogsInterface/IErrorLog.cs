using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace INFRAESTRUCTURE
{
    public interface IErrorLog
    {
        Task Write(string data);
    }
} 