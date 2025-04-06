repo := "myrepo"

build:
  #!/bin/bash
  set -euxo pipefail
  dotnet build GitSharp.sln -c Debug

clean:
  #!/bin/bash
  set -euxo pipefail
  rm -rf {{ repo }}

init: build
  #!/bin/bash
  set -euxo pipefail
  dotnet GitSharp/bin/Debug/net9.0/GitSharp.dll init {{ repo }}
