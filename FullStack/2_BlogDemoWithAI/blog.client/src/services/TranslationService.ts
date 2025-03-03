import type { PostTranslation } from "../interfaces/Post";

export class TranslationServiceError extends Error {
    constructor(message: string) {
        super(message);
        this.name = 'TranslationServiceError';
    }
}

export default class TranslationService {

    public async get(post: PostTranslation, lang: TranslationLanguage): Promise<PostTranslation> {
        const body = {
            title: post.title,
            content: post.content,
            targetLang: lang
        }

        const response = await fetch('/api/Translation', {
            method: 'POST',
            body: JSON.stringify(body),
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new TranslationServiceError(response.statusText);
        }

        return await response.json();
    }

}

export enum TranslationLanguage {
    Hungarian = 'HU',
    Spanish = 'ES',
    French = 'FR',
    German = 'DE',
    Italian = 'IT',
}
