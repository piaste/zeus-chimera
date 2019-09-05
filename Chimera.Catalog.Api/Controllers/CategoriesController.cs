using Chimera.Catalog.Api.Helpers;
using Chimera.Catalog.Api.Models;
using Chimera.Catalog.Api.Models.Requests;
using Chimera.Catalog.Api.Models.Responses;
using Chimera.Catalog.Entities;
using Chimera.Catalog.ServiceLayers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Extensions;

namespace Chimera.Catalog.Api.Controllers
{
    [Route("api/Categories")]
    public class CategoriesController : ApiControllerBase
    {
        /// <summary>
        /// Executes count of total categories on storage
        /// </summary>
        /// <returns>Returns count</returns>
        [HttpGet]
        [Route("Count")]
        [ProducesResponseType(200, Type= typeof(IntegerResponse))]
        public IActionResult Count()
        {
            //Invoke del service layer
            var count = CatalogLayer.CountCategories();
            return Ok(new IntegerResponse { Value = count });
        }

        /// <summary>
        /// Executes fetch of categories on storage
        /// with paging of data
        /// </summary>
        /// <returns>Returns list of elements</returns>
        [HttpPost]
        [Route("Fetch")]
        [ProducesResponseType(200, Type = typeof(List<CategoryContract>))]
        public IActionResult Fetch([FromBody]FetchPagedRequest request)
        {
            //Ipotesi: https://locahost:12345/api/Categories/Fetch?startRowIndex=0&maximumRows=10

            //Validazione della request
            if (request == null)
                return BadRequest();

            //Validazione del modello di request
            if (!ModelState.IsValid)
                return BadRequest();

            //Invoke del layer applicativo
            IList<Category> entities =  CatalogLayer.FetchCategories(
                request.StartRowIndex, request.MaximumRows);

            //Generazione dei contratti
            var contracts = new List<CategoryContract>();
            entities.Each(e => contracts.Add(ContractUtils.GenerateContract(e)));
            return Ok(contracts);
        }
    }
}
