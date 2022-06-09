import React, { useState, useEffect } from 'react';
import getAxios from '../AuthAxios';
import { axios } from 'axios';
import { useAuthContext } from '../AuthContext';
import { Link } from 'react-router-dom';

const Home = () => {
    const [joke, setJoke] = useState([]);
    const [jokeId, setId] = useState(0);
    const [disableLike, setDisableLike] = useState(false);
    const [disableDislike, setDisableDislike] = useState(false);
    const [likes, setLikes] = useState('');
    const [dislikes, setDislikes] = useState('');
    const { user } = useAuthContext();
    const { setup, punchline, id} = joke;


    const likeJoke = async () => {
        await getAxios().post(`/api/jokes/likejoke?id=${id}`);
    }
    const dislikeJoke = async () => {
        await getAxios().post(`/api/jokes/dislikejoke?id=${id}`);
    }

    const getLikes = async () => {
        const { data } = await getAxios().get(`api/jokes/getlikes?id=${id}`);
        setLikes(data);
    }
    const getDislikes = async () => {
        const { data } = await getAxios().get(`api/jokes/getdislikes?id=${id}`);
        setDislikes(data);
    }
    const interval = setInterval(async () => {
        if (jokeId) {
            const { data } = await getAxios().get(`/api/jokes/getlikeordislike?id=${jokeId}`);
            setDisableLike(data.liked);
            setDisableDislike(!data.liked);
        }
        getLikes();
        getDislikes();
    }, 500)
    useEffect(() => {
        const getJoke = async () => {
            const { data } = await getAxios().get('/api/jokes/getjoke');
            setJoke(data);
            setId(data.id);
            console.log(data);
        }
        getJoke();
    }, []);

    return (<div>
        <div className='card card-body bg-light col-md-6 offset-md-3'>
            <h5>{setup}</h5>
            <h5>{punchline}</h5>
            {!user && <div>
                <Link to='/login'>Login to your account to like/dislike this joke</Link></div>}
                <h5>Likes: {likes}</h5>
        <h5>Dislikes: {dislikes}</h5>
        {user && <div>
            <button className='btn btn-primary' disabled={disableLike} onClick={likeJoke}>Like</button>
            <button className='btn btn-danger' disabled={disableDislike} onClick={dislikeJoke}>Dislike</button>
        </div>}
        <h4>
            <button onClick={() => window.location.reload(false)} className='btn btn-link'>Refresh</button>
        </h4>
        </div>       
    </div>);
}

export default Home;