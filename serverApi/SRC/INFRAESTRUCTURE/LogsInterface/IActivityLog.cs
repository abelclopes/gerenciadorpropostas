using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace INFRAESTRUCTURE
{
    public interface IActivityLog
    {
        Task Write(string data);
    }
} 