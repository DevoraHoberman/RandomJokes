import React, { useState, useEffect } from 'react';
import getAxios from '../AuthAxios';
import JokeCards from '../components/JokeCards';

const ViewAll = () => {
    const [jokes, setJokes] = useState([]);
    useEffect(() => {
        const getJokes = async () => {
            const { data } = getAxios().get('api/jokes/getjokes');
            setJokes(data);
        }
        getJokes();
    }, [])
    return (<div>
        <div className='container'>
            {jokes && jokes.map((j) => {
               return <JokeCards
                    joke={j}
                    id={j.id} />
            })
            }
        </div>
    </div>);
}

export default ViewAll;