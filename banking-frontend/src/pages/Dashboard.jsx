// src/pages/Dashboard.jsx
import React, { useEffect, useState } from 'react';
import api from '../api/axios';

const Dashboard = () => {
    const [summary, setSummary] = useState({
        accounts: 0,
        balance: 0,
        loans: 0
    });

    useEffect(() => {
        const fetchData = async () => {
            try {
                // 1) Fetch accounts
                const { data: accounts } = await api.get('/account');
                // 2) Fetch loans for this user
                const { data: loans } = await api.get('/loan/my');

                const totalBalance = accounts.reduce((acc, a) => acc + a.balance, 0);
                setSummary({
                    accounts: accounts.length,
                    balance: totalBalance,
                    loans: loans.length
                });
            } catch (err) {
                console.error(err);
            }
        };
        fetchData();
    }, []);

    return (
        <div style={{ padding: 20 }}>
            <h2>Dashboard</h2>
            <div style={gridStyle}>
                <Card label="Total Accounts" value={summary.accounts} />
                <Card label="Total Balance" value={`₹${summary.balance}`} />
                <Card label="My Loans" value={summary.loans} />
            </div>
        </div>
    );
};

const gridStyle = {
    display: 'grid',
    gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))',
    gap: '20px',
    marginTop: '20px'
};

const Card = ({ label, value }) => (
    <div style={{
        padding: '20px',
        border: '1px solid #ccc',
        borderRadius: '8px',
        textAlign: 'center',
        background: '#f9f9f9'
    }}>
        <h3>{label}</h3>
        <p style={{ fontSize: '24px', margin: '10px 0' }}>{value}</p>
    </div>
);

export default Dashboard;
