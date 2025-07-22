// src/pages/Accounts.jsx
import React, { useEffect, useState } from 'react';
import api from '../api/axios';
import { Link } from 'react-router-dom';

const Accounts = () => {
    const [accounts, setAccounts] = useState([]);

    useEffect(() => {
        api.get('/account')
            .then(res => setAccounts(res.data))
            .catch(err => console.error(err));
    }, []);

    return (
        <div style={{ padding: 20 }}>
            <h2>My Accounts</h2>
            <table style={{ width: '100%', borderCollapse: 'collapse', marginTop: 10 }}>
                <thead>
                    <tr>
                        <th style={th}>Account Number</th>
                        <th style={th}>Balance</th>
                        <th style={th}>Type</th>
                        <th style={th}>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {accounts.map(a => (
                        <tr key={a.id}>
                            <td style={td}>{a.accountNumber}</td>
                            <td style={td}>₹{a.balance}</td>
                            <td style={td}>{a.accountType}</td>
                            <td style={td}>
                                <Link to={`/transactions/${a.id}`}>View Transactions</Link>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

const th = { border: '1px solid #ddd', padding: '8px', background: '#eee' };
const td = { border: '1px solid #ddd', padding: '8px' };

export default Accounts;

