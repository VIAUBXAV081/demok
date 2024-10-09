import { Button, Container, Form } from "react-bootstrap";
import { NewPost } from "../interfaces/Post";
import PostService from "../services/PostService";
import { Formik } from "formik";
import * as yup from 'yup';
import { useNavigate } from "react-router-dom";
import SuggestionService from "../services/SuggestionService";

function CreatePage() {

    const navigate = useNavigate();

    const initialValues: NewPost = {
        title: "",
        content: ""
    };

    const validationSchema = yup.object().shape({
        title: yup.string().required("Title is required!"),
        content: yup.string().required("Content is required!")
    });

    async function createPost(post: NewPost) {
        const service = new PostService();
        const created = await service.create(post);
        navigate('/post/' + created.id);
    }

    async function suggestPost(title: string, setFieldValue: (field: string, value: string) => void) {
        const service = new SuggestionService();
        const suggestion = await service.get({ title: title });
        setFieldValue("content", suggestion.content);
    }

    return (

        <Container>
            <h1 className="my-4">Create new post</h1>

            <Formik onSubmit={createPost} initialValues={initialValues} validationSchema={validationSchema}>
                {({ handleSubmit, handleChange, values, touched, errors, setFieldValue}) => (
                    <Form onSubmit={handleSubmit} noValidate>
                        <Form.Group className="mb-3" controlId="formTitle">
                            <Form.Label>Title</Form.Label>
                            <Form.Control type="text"
                                placeholder="Enter title"
                                name="title"
                                value={values.title}
                                onChange={handleChange}
                                isInvalid={touched.title && !!errors.title}
                            />
                            <Form.Control.Feedback type="invalid">
                                {errors.title}
                            </Form.Control.Feedback>
                        </Form.Group>


                        <Form.Group className="mb-3" controlId="formContent">
                            <Form.Label>Content</Form.Label>
                            <Form.Control as="textarea"
                                rows={3}
                                placeholder="Enter content"
                                name="content"
                                value={values.content}
                                onChange={handleChange}
                                isInvalid={touched.content && !!errors.content}
                            />
                            <Form.Control.Feedback type="invalid">
                                {errors.content}
                            </Form.Control.Feedback>
                        </Form.Group>

                        <div className="d-flex gap-2">
                            <Button variant="secondary" type="submit">
                                Save
                            </Button>

                            <Button variant="link" onClick={() => suggestPost(values.title, setFieldValue)} disabled={ !values.title }>
                                Suggest content <i className="bi bi-magic"></i>
                            </Button>
                        </div>
                    </Form>
                )}
            </Formik>

        </Container>



    );
}

export default CreatePage;