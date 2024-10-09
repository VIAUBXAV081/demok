interface Post {
    id: number;
    title: string;
    content: string;
}

function PostFull(props: { post: Post }) {

    return (
        <div className="post">
            <h2>{props.post.title}</h2>
            <p>{props.post.content} </p>
        </div>
    );
}

export default PostFull;