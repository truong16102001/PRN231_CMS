using BusinessObject.DTO;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IUploadRepository
    {
        Task<bool> AddUpload(Upload newCourse);
        Task<Upload> GetUploadById(int uploadId);
        Task<IEnumerable<Upload>> GetUploadsByRegistrationId(int registrationId);
        Task<bool> UpdateUpload(UploadAddUpdateDTO uploadAddUpdateDTO);
    }
}
