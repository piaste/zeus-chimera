﻿using Chimera.Catalog.Api.Helpers;
using Chimera.Catalog.Api.Models;
using Chimera.Catalog.Api.Models.Requests;
using Chimera.Catalog.Api.Models.Responses;
using Chimera.Catalog.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ZenProgramming.Chakra.Core.Extensions;

namespace Chimera.Catalog.Api.Controllers
{
    [Authorize]
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

            //Se l'utente non è amministratore non può richiedere più di 10 record alla volta
            if (User.Claims.Single(e => e.Type == "IsAdministrator").Value != "True" &&
                request.MaximumRows > 10)
            {
                //Inserimento di BadRequest
                ModelState.AddModelError("", "You cannot request more then 10 records");
                return BadRequest(ModelState);
            }

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
