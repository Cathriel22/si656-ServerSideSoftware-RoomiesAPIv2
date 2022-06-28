using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Roomies.API.Domain.Models;
using Roomies.API.Domain.Services;
using Roomies.API.Resources;

namespace Roomies.API.Controllers
{
    [Route("/api/landlords/{landlordId}/posts")]
    public class LandlordPostsController
    {      
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public LandlordPostsController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<PostResource>> GetAllByLandlordIdAsync(int landlordId)
        {
            var posts = await _postService.ListByLandlordIdAsync(landlordId);
            var resources = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResource>>(posts);

            return resources;
        }
    }
}
