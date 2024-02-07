import { Icon, InputAdornment, Stack, TextField, Typography } from "@mui/material";
import { LoadingButton } from "@mui/lab"
import { useState } from "react";

const FormLogin = (props) => {
	const [email, setEmail] = useState('');
	const [senha, setSenha] = useState('');

	function chamarLogin() { props.submit(email, senha); }

	return (
		<Stack display="flex" alignItems="center" justifyContent="center" minHeight="35em" gap="1em" p="0 10em">
			<Typography variant="h5" color="primary">Entrar no DesenVolve</Typography>
			<Typography variant="caption">Entre com sua conta DesenVolve</Typography>
				
			<TextField fullWidth label="E-mail" InputProps={{startAdornment: 
				(<InputAdornment position="start"><Icon>email</Icon></InputAdornment>)}}
				value={email} onChange={(e) => setEmail(e.target.value)} 
			/>
			<TextField fullWidth label="Senha" type="password" InputProps={{startAdornment: 
				(<InputAdornment position="start"><Icon>password</Icon></InputAdornment>)}}
				value={senha} onChange={(e) => setSenha(e.target.value)}
			/>
			<LoadingButton loading={props.loading} fullWidth variant="outlined"
				disabled={email.length == 0 || senha.length == 0} onClick={chamarLogin}
			>
				Entrar
			</LoadingButton>
		</Stack>
	);
};

export default FormLogin;
 