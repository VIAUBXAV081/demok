import { Link } from "react-router-dom";

interface Post {
    id: number;
    title: string;
    content: string;
}

function PostHead(props: { post: Post }) {

    return (
        <div className="post">
            <h2>{props.post.title}</h2>
            <p>{props.post.content.substring(0, 50) + " ..."} </p>
            <Link to={"/post/" + props.post.id}>Read more</Link>
        </div>
    );
}

export default PostHead;