import { Fragment, useEffect, useState } from 'react';
import PostHead from './PostHead';
import type Post from '../interfaces/Post';
import PostService from '../services/PostService';

function PostList() {

    const [posts, setPosts] = useState<Post[]>();

    useEffect(() => {
        populatePosts();
    }, []);

    async function populatePosts() {
        const service = new PostService();
        const data = await service.get();
        setPosts(data.reverse());
    }

    return (
        <div className="mb-4">
            {
                posts === undefined ?
                    <div>Loading posts</div> : (posts.length === 0 ?
                    <div>No posts</div> :
                    posts.map((post, index) => (
                        <Fragment key={post.id}>
                            <PostHead post={post} key={post.id} />
                            {index < posts.length - 1 ? <hr /> : null}
                        </Fragment>
                    )))
            }
        </div>
    );
}

export default PostList;