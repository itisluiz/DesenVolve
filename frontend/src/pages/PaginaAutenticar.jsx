import { Container, Paper, Tabs, Tab } from '@mui/material';
import { useContext, useEffect, useState } from 'react';
import { ContexoUsuario } from '../contexts/ContextoUsuario';
import { apiLogado, apiCadastro, apiLogin } from '../api/usuario';
import { useLocation, useNavigate } from 'react-router-dom';
import FormLogin from '../components/FormsAutenticar/FormLogin';
import FormCadastro from '../components/FormsAutenticar/FormCadastro';

const PaginaAutenticar = (props) => {
	const location = useLocation();
	const navigate = useNavigate();
	
	const {setUsuario} = useContext(ContexoUsuario);
	const [aba, setAba] = useState(0);
	const [carregando, setCarregando] = useState(false);

	function contextualizarUsuario(usr)
	{
		setCarregando(false);

		if (!usr)
			return;

		setUsuario(usr);

		let redirecionar = location.state?.from ?? '/';
		navigate(redirecionar);
	}
	
	useEffect(() => { apiLogado().then(usr => contextualizarUsuario(usr)) }, []);
	
	function submitLogin(email, senha)
	{
		setCarregando(true);
		apiLogin(email, senha).then(usr => contextualizarUsuario(usr));
	}

	function submitCadastro(nome, sobrenome, email, senha)
	{
		setCarregando(true);
		apiCadastro(nome, sobrenome, email, senha).then(usr => contextualizarUsuario(usr));
	}

	return (
		<Container sx={{ display: 'flex', justifyContent: 'center' }}>
		<Paper sx={{ width: '60%', mt: '3em' }}>
			<Tabs
				value={aba}
				onChange={(e, value) => {setAba(value)}}
				indicatorColor="primary"
				textColor="primary"
				variant="fullWidth"
			>
				<Tab value={0} label="Login" />
				<Tab value={1} label="Cadastro" />
			</Tabs>
			{aba === 0 && (<FormLogin loading={carregando} submit={submitLogin} />)}
			{aba === 1 && (<FormCadastro loading={carregando} submit={submitCadastro} />)}
		</Paper>
	  </Container>
	);
};

export default PaginaAutenticar;
