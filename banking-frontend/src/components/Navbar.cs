// src/components/Navbar.jsx
import React from 'react';
import { Link, useNavigate } from 'react-router-dom';

const Navbar = () => {
    const navigate = useNavigate();
    const token = localStorage.getItem('token');

    const handleLogout = () => {
        localStorage.removeItem('token');
        navigate('/login');
    };

    return (
        <nav style={styles.nav}>
            <h1 style={styles.logo}>BankingApp</h1>
            <div style={styles.links}>
                {!token ? (
                    <>
                        <Link to="/login" style={styles.link}>Login</Link>
                        <Link to="/register" style={styles.link}>Register</Link>
                    </>
                ) : (
                    <>
                        <Link to="/dashboard" style={styles.link}>Dashboard</Link>
                        <Link to="/accounts" style={styles.link}>Accounts</Link>
                        <Link to="/transfer" style={styles.link}>Transfer</Link>
                        <Link to="/loans" style={styles.link}>Loans</Link>
                        <button onClick={handleLogout} style={styles.button}>Logout</button>
                    </>
                )}
            </div>
        </nav>
    );
};

const styles = {
    nav: {
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        background: '#1976d2',
        padding: '10px 20px',
        color: '#fff'
    },
    logo: { margin: 0 },
    links: { display: 'flex', gap: '15px' },
    link: { color: '#fff', textDecoration: 'none' },
    button: {
        background: 'transparent',
        border: '1px solid #fff',
        color: '#fff',
        padding: '5px 10px',
        cursor: 'pointer'
    }
};

export default Navbar;
