import {
  Avatar,
  Box,
  Button,
  Divider,
  Icon,
  Stack,
  Typography,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Select,
  MenuItem,
} from "@mui/material";
import { useContext, useEffect, useState } from "react";
import { ContexoUsuario } from "../../contexts/ContextoUsuario";
import { LoadingButton } from "@mui/lab";
import { apiAlterarCargo, apiRemoverMembro } from "../../api/equipe";

const CardMembro = (props) => {
  const { usuario } = useContext(ContexoUsuario);

  const [editarMembroAberto, setEditarMembroAberto] = useState(false);
  const [novoCargo, setNovoCargo] = useState(props.membro.cargo.codigo);

  useEffect(() => {
    props.fnOnPopup(editarMembroAberto);
  }, [editarMembroAberto]);

  let cargoCor = undefined;
  let cargoIcone = "account_circle";

  if (props.membro.cargo.codigo == 2) {
    cargoCor = "primary";
    cargoIcone = "security";
  } else if (props.membro.cargo.codigo == 1) {
    cargoCor = "secondary";
    cargoIcone = "admin_panel_settings";
  }

  function temPermissaoSobre() {
    if (usuario.codigo == props.membro.usuario.codigo) return false;

    return props.cargoUsuario.codigo >= props.membro.cargo.codigo;
  }

  function cancelarEditarMembro() {
    setEditarMembroAberto(false);
    setNovoCargo(props.membro.cargo.codigo);
  }

  function submitAlterarCargo() {
    apiAlterarCargo(
      props.codigoEquipe,
      props.membro.usuario.codigo,
      novoCargo
    ).then((res) => {
      cancelarEditarMembro();
      props.fnAttMembros();
    });
  }

  function submitRemoverMembro() {
    apiRemoverMembro(props.codigoEquipe, props.membro.usuario.codigo).then(
      (res, ok) => {
        cancelarEditarMembro();
        props.fnAttMembros();
      }
    );
  }

  return (
    <>
      <Stack
        direction="column"
        spacing={0}
        sx={{ width: "100%", mb: "-0.7em" }}
      >
        <Divider variant="fullWidth" />

        <Box display="flex" justifyContent="start" alignItems="center">
          <Avatar
            sx={{ width: "1em", height: "1em", mr: "0.3em" }}
            src={props.membro.usuario.urlAvatar}
          />
          <Typography color={cargoCor} variant="h6" component="h2">
            {props.membro.usuario.nome} {props.membro.usuario.sobrenome}
          </Typography>
          {usuario.codigo == props.membro.usuario.codigo && (
            <Typography ml="0.4em">(você)</Typography>
          )}
        </Box>

        <Typography variant="caption" component="p">
          <Icon
            color={cargoCor}
            sx={{ fontSize: "1.4em", mb: "-0.2em", mr: "0.2em" }}
          >
            {cargoIcone}
          </Icon>
          {props.membro.cargo.nome}
        </Typography>
      </Stack>
      {temPermissaoSobre() && (
        <>
          <Button
            onClick={() => setEditarMembroAberto(true)}
            variant="outlined"
            sx={{ mt: "-0.95em" }}
          >
            <Icon sx={{ fontSize: "1.2em" }}>build</Icon>
          </Button>
          <Dialog open={editarMembroAberto} onClose={cancelarEditarMembro}>
            <DialogTitle>Editar opções de membro</DialogTitle>
            <DialogContent>
              <Typography variant="caption" component="p">
                Alterando opções de{" "}
                <b>
                  {props.membro.usuario.nome} {props.membro.usuario.sobrenome}
                </b>
              </Typography>
              <Typography variant="caption" component="p">
                E-mail: <b>{props.membro.usuario.email}</b>
              </Typography>

              {props.cargoUsuario.codigo == 2 && (
                <>
                  <Typography sx={{ mt: "1em" }}>Alterar cargo</Typography>
                  <Select
                    fullWidth
                    value={novoCargo}
                    onChange={(e) => setNovoCargo(e.target.value)}
                  >
                    <MenuItem value={0}>Membro</MenuItem>
                    <MenuItem value={1}>Admnistrador</MenuItem>
                    <MenuItem value={2}>Líder</MenuItem>
                  </Select>
                </>
              )}
            </DialogContent>
            <DialogActions>
              <Button onClick={cancelarEditarMembro}>Cancelar</Button>
              <LoadingButton color="error" onClick={submitRemoverMembro}>
                Remover membro
              </LoadingButton>
              {props.cargoUsuario.codigo == 2 && (
                <LoadingButton onClick={submitAlterarCargo}>
                  Alterar cargo
                </LoadingButton>
              )}
            </DialogActions>
          </Dialog>
        </>
      )}
    </>
  );
};

export default CardMembro;
