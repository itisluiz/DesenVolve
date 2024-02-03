import subprocess
import shutil
import json

def removerMigracoes():
	shutil.rmtree('Migrations', ignore_errors=True)

def forcarDropDatabase(server, database):
	return subprocess.call(f'sqlcmd -S "{server}" -E -Q "USE MASTER;ALTER DATABASE {database} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;USE MASTER;DROP DATABASE {database};"', shell=True, stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)

def criarMigracao(migracao):
	return subprocess.call(f'dotnet ef migrations add {migracao}', shell=True)

def atualizarBanco():
	return subprocess.call('dotnet ef database update', shell=True)

server = None
database = None

if __name__ == '__main__':
	try:
		with open('appsettings.json', 'r') as f:
			data = json.load(f)
			dbconnection = data.get('DBConnection', {})
			server = dbconnection.get('Server')
			database = dbconnection.get('Database')
	except Exception:
		print(f'Falha ao ler appsettings.json')
		exit

	
	print(f'\033[92mForçando drop do banco de dados "\033[94m{database}\033[92m" no servidor "\033[94m{server}\033[92m"\033[0m')
	forcarDropDatabase(server, database)
	print('\033[92mRemovendo migrações existentes\033[0m')
	removerMigracoes()
	print('\033[92mCriando migração "\033[94mBase\033[92m"\033[0m')
	criarMigracao('Base')
	print('\033[92mAtualizando banco de dados\033[0m')
	atualizarBanco()
