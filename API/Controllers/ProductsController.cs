
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
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
        public  async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsrepo.ListAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product= await _productsrepo.GetEntitywithspec(spec);
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