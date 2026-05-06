param(
    [Parameter(Position = 0)]
    [string]$Task = "build",

    [Parameter(Position = 1)]
    [string]$Arg
)

function env {
    Copy-Item ".env.tmpl" ".env" -Force
}

function build {
    env
    dotnet build
}

function dev {
    dotnet watch --project API --no-hot-reload
}

function migrate {
    param(
        [string]$Name
    )

    if (-not $Arg) {
        Write-Host "Please provide a migration name"
        exit 1
    }

    # Add new migration
    dotnet ef migrations add $Name `
        -s API `
        -p Infrastructure `
        -o Data/Migrations

    # Update database
    dotnet ef --project API database update
}

function remove_migrations {
    # Remove existing migration files
    Remove-Item "Infrastructure/Data/Migrations/*.cs" -Force -ErrorAction SilentlyContinue
}

switch ($Task) {
    "env" { env }
    "build" { build }
    "dev" { dev }
    "migrate" { migrate -Name $Arg }
    "remove_migrations" { remove_migrations }
    default { Write-Host "Unknown task: $Task" }
}