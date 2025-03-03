import { useEffect, useState } from 'react';
import type Post from '../interfaces/Post';
import PostService from '../services/PostService';
import { Link } from 'react-router-dom';

function RecentPosts() {

    const [posts, setPosts] = useState<Post[]>();

    useEffect(() => {
        populatePosts();
    }, []);

    async function populatePosts() {
        const service = new PostService();
        const data = await service.get();
        const reduced = data.reverse().slice(0, 5);
        setPosts(reduced);
    }

    return (
        <div className="bg-body-secondary px-4 py-2 rounded">
            <h4>Recent posts</h4>
            {posts === undefined ? <div>Loading posts</div> : (posts.length === 0 ?
                <div>No posts</div> :
                <ul>
                    {posts.map((post) => (
                        <li key={post.id}>
                            <Link to={`/post/${post.id}`}>{post.title.substring(0, 25) + " ..."}</Link>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}

export default RecentPosts;