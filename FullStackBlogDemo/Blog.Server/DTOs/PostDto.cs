﻿namespace Blog.Server.DTOs
{
    public record PostDto
    {
        public int? ID { get; init; }
        public string? Title { get; init; }
        public string? Content { get; init; }
    }
}
