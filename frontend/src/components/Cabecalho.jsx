import { AppBar, Toolbar, Typography, Stack, Icon } from '@mui/material';

const Cabecalho = () => {
	return (
		<AppBar position="sticky">
			<Toolbar>
				<Icon sx={{fontSize: '1.75em'}}>checklist</Icon>
				<Stack marginLeft="0.75em" spacing="-0.75em" flexGrow="1">
					<Typography fontWeight="bolder" variant="h6">DesenVolve</Typography>
					<Typography variant="caption">Sistema de gerenciamento e apoio para projetos</Typography>
				</Stack>
			</Toolbar>
		</AppBar>
	);
};

export default Cabecalho;
