import React, { useEffect, useState } from 'react';
import api from '../api/axios';

const Loans = () => {
    const [loans, setLoans] = useState([]);
    const [amount, setAmount] = useState('');
    const [interest, setInterest] = useState('');
    const [term, setTerm] = useState('');
    const [msg, setMsg] = useState('');

    useEffect(() => {
        api.get('/loan/my')
            .then(res => setLoans(res.data))
            .catch(err => console.error(err));
    }, []);

    const apply = async e => {
        e.preventDefault();
        setMsg('');
        try {
            const { data } = await api.post('/loan', {
                amount: parseFloat(amount),
                interestRate: parseFloat(interest),
                termInMonths: parseInt(term)
            });
            setMsg(data.message);
            setLoans(prev => [...prev, { id: data.loanId, amount, interestRate: interest, termInMonths: term, status: 'Pending' }]);
        } catch (err) {
            setMsg(err.response?.data || 'Error');
        }
    };

    return (
        <div style={{ padding: 20 }}>
            <h2>My Loans</h2>
            <form onSubmit={apply} style={{ display: 'flex', flexDirection: 'column', gap: 10, maxWidth: 400 }}>
                <input placeholder="Amount" type="number" value={amount} onChange={e => setAmount(e.target.value)} required />
                <input placeholder="Interest Rate" type="number" value={interest} onChange={e => setInterest(e.target.value)} required />
                <input placeholder="Term (months)" type="number" value={term} onChange={e => setTerm(e.target.value)} required />
                <button type="submit">Apply</button>
            </form>
            {msg && <p>{msg}</p>}

            <h3>Existing Applications</h3>
            <table style={{ width: '100%', border: '1px solid #ccc', borderCollapse: 'collapse' }}>
                <thead><tr><th>ID</th><th>Amt</th><th>Rate</th><th>Term</th><th>Status</th></tr></thead>
                <tbody>
                    {loans.map(l => (
                        <tr key={l.id}>
                            <td>{l.id}</td>
                            <td>₹{l.amount}</td>
                            <td>{l.interestRate}%</td>
                            <td>{l.termInMonths}</td>
                            <td>{l.status}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default Loans;
