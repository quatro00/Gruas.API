﻿using Gruas.API.Models;
using Gruas.API.Models.DTO.Reportes;

namespace Gruas.API.Repositories.Interface
{
    public interface IReportesRepository
    {
        public Task<ResponseModel> GetPagos(GetPagos_Request model);
    }
}
