﻿using Chimera.Catalog.Api.Helpers;
using Chimera.Catalog.Api.Models;
using Chimera.Catalog.Api.Models.Requests;
using Chimera.Catalog.Api.Models.Responses;
using Chimera.Catalog.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ZenProgramming.Chakra.Core.Extensions;

namespace Chimera.Catalog.Api.Controllers
{
    [Route("api/Products")]
    public class ProductsController : ApiControllerBase
    {
        /// <summary>
        /// Executes count of total categories on storage
        /// </summary>
        /// <returns>Returns count</returns>
        [HttpGet]
        [Route("Count")]
        [ProducesResponseType(200, Type = typeof(IntegerResponse))]
        public IActionResult Count()
        {
            //Invoke del service layer
            var count = CatalogLayer.CountProducts();
            return Ok(new IntegerResponse { Value = count });
        }

        /// <summary>
        /// Executes fetch of categories on storage
        /// with paging of data
        /// </summary>
        /// <returns>Returns list of elements</returns>
        [HttpPost]
        [Route("Fetch")]
        [ProducesResponseType(200, Type = typeof(List<ProductContract>))]
        public IActionResult Fetch([FromBody]FetchPagedRequest request)
        {
            //Validazione della request
            if (request == null)
                return BadRequest();

            //Validazione del modello di request
            if (!ModelState.IsValid)
                return BadRequest();

            //Invoke del layer applicativo
            IList<Product> products = CatalogLayer.FetchProducts(
                request.StartRowIndex, request.MaximumRows);
            var categoryIds = products.Select(e => e.CategoryId).ToArray();
            IList<Category> categories = CatalogLayer.FetchCategoriesByIds(categoryIds);

            //Generazione dei contratti
            var contracts = new List<ProductContract>();
            products.Each(e => contracts.Add(ContractUtils.GenerateContract(e, categories)));
            return Ok(contracts);
        }
    }
}