﻿namespace QuizIdentity.Application.DTOs.Account.Request;

public class CreateAccountRequestDto
{
    public required string Email { get; set; }

    public required string Password { get; set; }

    public required string ConfirmPassword { get; set; }
}
