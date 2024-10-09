import type Post from "../interfaces/Post";
import type { NewPostIdea } from "../interfaces/Post";

export class SuggestionServiceError extends Error {
    constructor(message: string) {
        super(message);
        this.name = 'SuggestionServiceError';
    }
}

export default class SuggestionService {

    public async get(post: NewPostIdea): Promise<Post> {
        const response = await fetch('/api/Suggestion', {
            method: 'POST',
            body: JSON.stringify(post),
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new SuggestionServiceError(response.statusText);
        }

        return await response.json();
    }

}
