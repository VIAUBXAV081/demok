import type Post from "../interfaces/Post";
import type { NewPost } from "../interfaces/Post";

export class PostServiceError extends Error {
    constructor(message: string) {
        super(message);
        this.name = 'PostServiceError';
    }
}

export default class PostService {

    public async get(): Promise<Post[]> {
        const response = await fetch('/api/Post');

        if (!response.ok) {
            throw new PostServiceError(response.statusText);
        }

        return await response.json();
    }

    public async getById(id: number): Promise<Post> {
        const response = await fetch('/api/Post/' + id);

        if (!response.ok) {
            throw new PostServiceError(response.statusText);
        }

        return await response.json();
    }

    public async create(post: NewPost): Promise<Post> {
        const response = await fetch('/api/Post/', {
            method: 'POST',
            body: JSON.stringify(post),
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new PostServiceError(response.statusText);
        }

        return await response.json();
    }

    public async delete(id: number): Promise<void> {
        const response = await fetch('/api/Post/' + id, {
            method: 'DELETE'
        });

        if (!response.ok) {
            throw new PostServiceError(response.statusText);
        }
    }

}
