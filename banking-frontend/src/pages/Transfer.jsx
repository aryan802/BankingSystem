import React, { useEffect, useState } from 'react';
import api from '../api/axios';

const Transfer = () => {
    const [accounts, setAccounts] = useState([]);
    const [from, setFrom] = useState('');
    const [to, setTo] = useState('');
    const [amount, setAmount] = useState('');
    const [desc, setDesc] = useState('');
    const [msg, setMsg] = useState('');

    useEffect(() => {
        api.get('/account').then(res => {
            setAccounts(res.data);
            if (res.data.length) setFrom(res.data[0].accountNumber);
        });
    }, []);

    const handle = async e => {
        e.preventDefault();
        setMsg('');
        try {
            const { data } = await api.post('/transfer', {
                fromAccountNumber: from,
                toAccountNumber: to,
                amount: parseFloat(amount),
                description: desc
            });
            setMsg(data.message);
        } catch (err) {
            setMsg(err.response?.data || 'Error');
        }
    };

    return (
        <div style={{ padding: 20 }}>
            <h2>Transfer Funds</h2>
            <form onSubmit={handle} style={{ display: 'flex', flexDirection: 'column', gap: 10, maxWidth: 400 }}>
                <select value={from} onChange={e => setFrom(e.target.value)}>
                    {accounts.map(a => <option key={a.id} value={a.accountNumber}>{a.accountNumber}</option>)}
                </select>
                <input placeholder="To Account" value={to} onChange={e => setTo(e.target.value)} required />
                <input placeholder="Amount" type="number" value={amount} onChange={e => setAmount(e.target.value)} required />
                <input placeholder="Description" value={desc} onChange={e => setDesc(e.target.value)} />
                <button type="submit">Send</button>
                {msg && <p>{msg}</p>}
            </form>
        </div>
    );
};

export default Transfer;
