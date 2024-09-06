using AutoMapper;
using BookStoreFullStackNg.Data.Reopositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreFullStackNg.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenreRepository _genreRepository;

        public GenreController(IMapper mapper, IGenreRepository genreRepository = null)
        {
            _mapper = mapper;
            _genreRepository = genreRepository;
        }
    }
}
