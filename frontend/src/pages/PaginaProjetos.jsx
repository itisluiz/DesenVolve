import {
  Container,
  Paper,
  Stack,
  Typography,
  Dialog,
  DialogContent,
  DialogTitle,
  Fab,
  Icon,
  TextField,
  DialogActions,
  Button,
  Drawer,
  List,
  ListItem,
  Box,
  ClickAwayListener,
} from "@mui/material";
import {
  apiProjetos,
  apiObterEquipe,
  apiAdicionarMembro,
  apiRemoverMembro,
  apiRemoverEquipe,
} from "../api/equipe";
import { LoadingButton } from "@mui/lab";
import { useContext, useEffect, useState } from "react";
import { apiCriarProjeto } from "../api/projeto";
import { enqueueSnackbar } from "notistack";
import { useNavigate, useParams } from "react-router-dom";
import { ContexoUsuario } from "../contexts/ContextoUsuario";

import CardProjeto from "../components/Cards/CardProjeto";
import CardMembro from "../components/Cards/CardMembro";

const PaginaProjetos = (props) => {
  const { codigo } = useParams();
  const navigate = useNavigate();
  const { usuario } = useContext(ContexoUsuario);

  const [criarProjetoAberto, setCriarProjetoAberto] = useState(false);
  const [adicionarMembroAberto, setAdicionarMembroAberto] = useState(false);
  const [sairEquipeAberto, setSairEquipeAberto] = useState(false);

  const [nomeNovoProjeto, setNomeNovoProjeto] = useState("");
  const [emailNovoMembro, setEmailNovoMembro] = useState("");

  const [projetosEquipe, setProjetosEquipe] = useState([]);
  const [equipe, setEquipe] = useState(null);
  const [carregandoCriarProjeto, setCarregandoCriarProjeto] = useState(false);
  const [carregandoAdicionarMembro, setCarregandoAdicionarMembro] =
    useState(false);

  const [drawerMembrosAberta, setDrawerMembrosAberta] = useState(false);

  const [bloquearClickaway, setBloquarClickaway] = useState(false);

  const [cargoUsuario, setCargoUsuario] = useState(null);

  function cancelarCriarProjeto() {
    setCriarProjetoAberto(false);
    setNomeNovoProjeto("");
  }
  function cancelarAdicionarMembro() {
    setAdicionarMembroAberto(false);
    setEmailNovoMembro("");
    setBloquarClickaway(false);
  }
  function cancelarSairEquipe() {
    setSairEquipeAberto(false);
  }

  async function atualizarProjetosEquipe() {
    apiProjetos(codigo).then((projetos) => {
      if (!projetos) navigate("/");

      setProjetosEquipe(projetos);
    });
  }
  async function atualizarDadosEquipe() {
    apiObterEquipe(codigo).then((equipe) => {
      if (!equipe) navigate("/");

      setEquipe(equipe);
      setCargoUsuario(
        equipe.membrosEquipe.find(
          (membro) => membro.usuario.codigo == usuario.codigo
        ).cargo
      );
    });
  }

  async function submitCriarProjeto() {
    setCarregandoCriarProjeto(true);
    apiCriarProjeto(codigo, nomeNovoProjeto).then((projeto) => {
      if (projeto) {
        enqueueSnackbar("Projeto criado com sucesso!", { variant: "success" });
        atualizarProjetosEquipe();
        cancelarCriarProjeto();
      }
      setCarregandoCriarProjeto(false);
    });
  }

  async function submitAdicionarMembro() {
    setCarregandoAdicionarMembro(true);
    apiAdicionarMembro(codigo, emailNovoMembro, 0).then(() => {
      atualizarDadosEquipe();
      cancelarAdicionarMembro();
      setCarregandoAdicionarMembro(false);
    });
  }

  async function submitSairEquipe() {
    if (cargoUsuario.codigo == 2) {
      apiRemoverEquipe(codigo).then(() => {
        navigate("/");
      });
      return;
    }

    apiRemoverMembro(codigo, usuario.codigo).then(() => {
      navigate("/");
    });
  }

  useEffect(() => {
    atualizarProjetosEquipe();
    atualizarDadosEquipe();
  }, [codigo]);

  return (
    <Container sx={{ display: "flex", mt: "1.5em" }}>
      <Drawer
        variant="temporary"
        open={drawerMembrosAberta}
        anchor="left"
        position="fixed"
        sx={{ zIndex: 99 }}
      >
        <ClickAwayListener
          onClickAway={() =>
            !bloquearClickaway && setDrawerMembrosAberta(false)
          }
        >
          <Box sx={{ width: "20em" }}>
            <List sx={{ mt: "3.5em" }}>
              <ListItem>
                <div>
                  <Typography variant="h6" color="primary" mb="-0.5em">
                    Membros
                  </Typography>
                  <Typography variant="caption">
                    Lista de membros da equipe
                  </Typography>
                </div>

                {cargoUsuario && cargoUsuario.codigo >= 1 && (
                  <Fab
                    onClick={() => {
                      setAdicionarMembroAberto(true);
                      setBloquarClickaway(true);
                    }}
                    sx={{ ml: "auto", width: "3em", height: "3em" }}
                    variant="contained"
                    color="primary"
                  >
                    <Icon>add</Icon>
                  </Fab>
                )}
              </ListItem>

              {equipe &&
                equipe.membrosEquipe.map((membro) => (
                  <ListItem key={membro.usuario.nome}>
                    <CardMembro
                      fnOnPopup={setBloquarClickaway}
                      fnAttMembros={atualizarDadosEquipe}
                      codigoEquipe={equipe.codigo}
                      membro={membro}
                      cargoUsuario={cargoUsuario}
                    />
                  </ListItem>
                ))}
            </List>
          </Box>
        </ClickAwayListener>
      </Drawer>

      <Button
        onClick={() => setDrawerMembrosAberta(true)}
        variant="contained"
        color="primary"
        sx={{ position: "fixed", top: "6em", left: "1em", zIndex: 98 }}
      >
        <Icon sx={{ mr: "0.3em" }}>group</Icon>Membros
      </Button>

      <Stack width="100%" gap="1em">
        <div style={{ display: "flex", justifyContent: "space-between" }}>
          <span style={{ display: "flex" }}>
            <Fab
              onClick={() => navigate("/")}
              variant="contained"
              sx={{ mr: "2em", zIndex: 0 }}
            >
              <Icon>arrow_left</Icon>
            </Fab>
            <Typography variant="h3" color="primary">
              <b>Equipe</b> {equipe?.nome}
            </Typography>
          </span>
          <span>
            <Fab
              onClick={() => setSairEquipeAberto(true)}
              variant="contained"
              color="error"
              sx={{ m: "0 0.5em", zIndex: 0 }}
            >
              <Icon>logout</Icon>
            </Fab>
            <Fab
              onClick={atualizarProjetosEquipe}
              variant="contained"
              color="primary"
              sx={{ m: "0 0.5em", zIndex: 0 }}
            >
              <Icon>refresh</Icon>
            </Fab>
            {cargoUsuario && cargoUsuario.codigo >= 1 && (
              <Fab
                onClick={() => setCriarProjetoAberto(true)}
                variant="contained"
                color="primary"
                sx={{ ml: "0.5em", zIndex: 0 }}
              >
                <Icon>add</Icon>
              </Fab>
            )}
          </span>
        </div>
        <Paper sx={{ height: "35em", width: "100%", overflow: "scroll" }}>
          {projetosEquipe.map((projeto) => (
            <CardProjeto key={projeto.nome} projeto={projeto} />
          ))}
        </Paper>
      </Stack>
      <Dialog open={criarProjetoAberto} onClose={cancelarCriarProjeto}>
        <DialogTitle>Criar novo projeto</DialogTitle>
        <DialogContent>
          <TextField
            value={nomeNovoProjeto}
            onChange={(e) => setNomeNovoProjeto(e.target.value)}
            autoFocus
            placeholder="Nome do projeto"
            label="Nome"
            fullWidth
            variant="standard"
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={cancelarCriarProjeto}>Cancelar</Button>
          <LoadingButton
            onClick={submitCriarProjeto}
            loading={carregandoCriarProjeto}
            disabled={nomeNovoProjeto.length < 2}
          >
            Criar
          </LoadingButton>
        </DialogActions>
      </Dialog>
      <Dialog open={adicionarMembroAberto} onClose={cancelarAdicionarMembro}>
        <DialogTitle>Adicionar membro na equipe</DialogTitle>
        <DialogContent>
          <TextField
            value={emailNovoMembro}
            onChange={(e) => setEmailNovoMembro(e.target.value)}
            autoFocus
            placeholder="E-mail do integrante"
            label="E-mail"
            fullWidth
            variant="standard"
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={cancelarAdicionarMembro}>Cancelar</Button>
          <LoadingButton
            onClick={submitAdicionarMembro}
            loading={carregandoAdicionarMembro}
            disabled={emailNovoMembro.length < 2}
          >
            Adicionar
          </LoadingButton>
        </DialogActions>
      </Dialog>

      <Dialog open={sairEquipeAberto} onClose={cancelarSairEquipe}>
        <DialogTitle color="error">Sair da equipe</DialogTitle>
        <DialogContent>
          <Typography variant="caption" component="p">
            Você tem certeza que deseja sair da equipe <b>{equipe?.nome}</b>?
          </Typography>
          {cargoUsuario?.codigo == 2 && (
            <Typography color="error" variant="h6" component="p">
              Você é líder da equipe, ao sair, a equipe será desfeita.
            </Typography>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={cancelarSairEquipe}>Cancelar</Button>
          <Button color="error" onClick={submitSairEquipe}>
            Sair
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default PaginaProjetos;
