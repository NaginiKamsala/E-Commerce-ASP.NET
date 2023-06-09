
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using API.Helpers;

namespace API.Controllers
{
    
    public class ProductsController : BaseApiController
    {

        private readonly IGenericRepository<Product> _productsrepo;
        private readonly IGenericRepository<ProductType> _producttyperepo;
        private readonly IGenericRepository<ProductBrand> _producbrandrepo;
        private readonly IMapper _mapper;
 
        public ProductsController(IGenericRepository<Product> productsrepo, IGenericRepository<ProductBrand> producbrandrepo,
         IGenericRepository<ProductType> producttyperepo, IMapper mapper)
        {
            _mapper = mapper;
            _producbrandrepo = producbrandrepo;
            _producttyperepo = producttyperepo;
            _productsrepo = productsrepo;


        }
        [HttpGet]
        public  async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productparams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productparams);
            var countSpec= new ProductWithFiltersForCountSpecification(productparams);
            var totalItems= await _productsrepo.CountAsync(countSpec);
            var products = await _productsrepo.ListAsync(spec);
            var data =_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
       
            return Ok(new Pagination<ProductToReturnDto>(productparams.PageIndex, productparams.PageSize, totalItems, data));
             }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product= await _productsrepo.GetEntitywithspec(spec);
            if(product==null) return NotFound(new ApiResponse(404));
            return  _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok( await  _producbrandrepo.ListAllAsync());
        }
         [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok( await  _producttyperepo.ListAllAsync());
        }
    }
}