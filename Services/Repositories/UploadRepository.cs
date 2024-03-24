using BusinessObject.DTO;
using BusinessObject.Models;
using Infrastructures.DataAccess;
using Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class UploadRepository : IUploadRepository
    {
        public Task<bool> AddUpload(Upload newCourse)
        {
            return UploadDAO.Add(newCourse);
        }

        public Task<Upload> GetUploadById(int uploadId)
        {
            return UploadDAO.GetById(uploadId);
        }

        public Task<IEnumerable<Upload>> GetUploadsByRegistrationId(int registrationId)
        {
            return UploadDAO.GetByRegistrationId(registrationId);
        }

        public Task<bool> UpdateUpload(UploadAddUpdateDTO uploadAddUpdateDTO)
        {
            return UploadDAO.Update(uploadAddUpdateDTO);
        }
    }
}
