import React, { useEffect, useState } from 'react';
import api from '../api/axios';

const Dashboard = () => {
    const [summary, setSummary] = useState({
        accounts: 0,
        balance: 0,
        loans: 0
    });

    useEffect(() => {
        const fetchSummary = async () => {
            try {
                // 1. fetch accounts
                const { data: accounts } = await api.get('/account');
                // 2. fetch loans (customer only)
                const { data: loans } = await api.get('/loan/my');

                const totalBalance = accounts.reduce((sum, acc) => sum + acc.balance, 0);
                setSummary({
                    accounts: accounts.length,
                    balance: totalBalance,
                    loans: loans.length
                });
            } catch (err) {
                console.error(err);
            }
        };
        fetchSummary();
    }, []);

    return (
        <div style={{ padding: 20 }}>
            <h2>Dashboard</h2>
            <div style={{ display: 'flex', gap: '20px' }}>
                <Card label="Accounts" value={summary.accounts} />
                <Card label="Total Balance" value={`₹${summary.balance}`} />
                <Card label="My Loans" value={summary.loans} />
            </div>
        </div>
    );
};

const Card = ({ label, value }) => (
    <div style={{
        border: '1px solid #ccc', borderRadius: 8,
        padding: 20, flex: 1, textAlign: 'center'
    }}>
        <h3>{label}</h3>
        <p style={{ fontSize: 24 }}>{value}</p>
    </div>
);

export default Dashboard;
