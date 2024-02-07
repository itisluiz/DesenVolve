import { AppBar, Toolbar, Typography, Stack, Icon, Avatar } from '@mui/material';
import { useContext } from 'react';
import { ContextoUsuario } from '../contexts/ContextoUsuario';
import { Navigate } from 'react-router-dom';

const Cabecalho = () => {
	const {usuario, setUsuario} = useContext(ContextoUsuario);

	return (
		<AppBar position="sticky">
			<Toolbar>
				<Icon sx={{fontSize: '1.75em'}}>checklist</Icon>
				<Stack marginLeft="0.75em" spacing="-0.75em" flexGrow="1">
					<Typography fontWeight="bolder" variant="h6">DesenVolve</Typography>
					<Typography variant="caption">Sistema de gerenciamento e apoio para projetos</Typography>
				</Stack>
				{usuario &&
				<>
					<Typography fontSize='1.1em'>{usuario.nome}</Typography>
					<Avatar sx={{width: '1.5em', height: '1.5em', ml: '0.5em'}}></Avatar>
				</>}
			</Toolbar>
		</AppBar>
	);
};

export default Cabecalho;
