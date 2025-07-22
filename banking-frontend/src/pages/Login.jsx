import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import { useAuth } from '../auth/AuthContext';

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        try {
            const res = await api.post('/auth/login', { email, password });
            const { token } = res.data;
            login(token);                // Set auth state
            navigate('/dashboard');      // Redirect
        } catch (err) {
            console.error(err);
            setError('Invalid email or password.');
        }
    };

    return (
        <div style={styles.container}>
            <h2>Login</h2>
            <form onSubmit={handleSubmit} style={styles.form}>
                <input
                    type="email"
                    placeholder="Email"
                    value={email}
                    required
                    onChange={(e) => setEmail(e.target.value)}
                    style={styles.input}
                />
                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    required
                    onChange={(e) => setPassword(e.target.value)}
                    style={styles.input}
                />
                {error && <p style={styles.error}>{error}</p>}
                <button type="submit" style={styles.button}>Login</button>
            </form>
        </div>
    );
};

export default Login;

// ───────────────────────────────────────────────────────
// Styles object used in the component above
const styles = {
    container: {
        width: '300px',
        margin: '60px auto',
        textAlign: 'center',
    },
    form: {
        display: 'flex',
        flexDirection: 'column',
        gap: '10px',
    },
    input: {
        padding: '8px',
        fontSize: '16px',
    },
    button: {
        padding: '10px',
        background: '#4CAF50',
        color: 'white',
        border: 'none',
        cursor: 'pointer',
    },
    error: {
        color: 'red',
    },
};

