using CP.Api.Services;

using Microsoft.AspNetCore.Mvc;

namespace CP.Api.Controllers;

public class VoteController : ControllerBase
{
    private readonly IVoteService _voteService;
}