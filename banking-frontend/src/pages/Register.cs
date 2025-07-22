import React, { useState } from 'react';
import api from '../api/axios';
import { useNavigate } from 'react-router-dom';

const Register = () => {
    const [fullName, setFullName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [msg, setMsg] = useState('');
    const navigate = useNavigate();

    const handle = async (e) => {
        e.preventDefault();
        setMsg('');
        try {
            await api.post('/auth/register', { fullName, email, password });
            setMsg('Registered! Redirecting to login...');
            setTimeout(() => navigate('/login'), 1500);
        } catch (err) {
            setMsg(err.response?.data || 'Error');
        }
    };

    return (
        <div style={{ padding: 20, maxWidth: 400, margin: 'auto' }}>
            <h2>Register</h2>
            <form onSubmit={handle} style={{ display: 'flex', flexDirection: 'column', gap: 10 }}>
                <input placeholder="Full Name" value={fullName} onChange={e => setFullName(e.target.value)} required />
                <input type="email" placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} required />
                <input type="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)} required />
                <button type="submit">Register</button>
                {msg && <p>{msg}</p>}
            </form>
        </div>
    );
};

export default Register;
