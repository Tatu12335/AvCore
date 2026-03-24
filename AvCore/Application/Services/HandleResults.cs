using AvCore.Application.DTOs;
using AvCore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvCore.Application.Services
{
    public class HandleResults : IHandleResults
    {
        public async Task<string> HandleResult(Response response)
        {
            if (response.QueryStatus == "ok" && response.Data?.Count > 0)
            {
                var malwareData = response.Data[0];
                return $"Detection! : Signature : {malwareData.Signature}, File : {malwareData.FileName} ";
            }
            else
            {
                return "Clean";
            }
        }

    }
}
