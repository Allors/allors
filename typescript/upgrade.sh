cd ..

npx create-nx-workspace@latest allors --preset=apps --nxCloud=false

cd allors

npm install -D jest-chain
npm install -D jest-trx-results-processor
npm install -D @nx/angular

npm install @angular/cdk
npm install @angular/material
npm install @angular/material-luxon-adapter
npm install bootstrap@4.6.0
npm install common-tags
npm install cross-fetch
npm install date-fns
npm install easymde
npm install jsnlog
npm install luxon

# Workspace
# Base
npx nx g @nx/angular:application --projectNameAndRootFormat=as-provided --name=workspace-base-angular-foundation-app --directory=workspace/base/angular-foundation-app --bundler=esbuild --routing=true --ssr=false --standalone=false --style=scss --skipTests=true --e2eTestRunner=none --minimal=true --strict=false
npx nx g @nx/angular:application --projectNameAndRootFormat=as-provided --name=workspace-base-angular-material-app --directory=workspace/base/angular-material-app --bundler=esbuild --routing=false --ssr=false --standalone=false --style=scss --skipTests=true --e2eTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-base-angular-foundation --directory=workspace/base/angular-foundation --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-base-angular-application --directory=workspace/base/angular-application --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-base-angular-material-foundation --directory=workspace/base/angular-material-foundation --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-base-angular-material-application --directory=workspace/base/angular-material-application --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-base-domain --directory=workspace/base/domain --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-base-meta --directory=workspace/base/meta --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-base-meta-json --directory=workspace/base/meta-json --bundler=none --unitTestRunner=none --minimal=true --strict=false

# System
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-system-adapters --directory=workspace/system/adapters --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-system-adapters-tests --directory=workspace/system/adapters-tests --bundler=none --unitTestRunner=jest --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-system-adapters-json --directory=workspace/system/adapters-json --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-system-adapters-json-tests --directory=workspace/system/adapters-json-tests --bundler=none --unitTestRunner=jest --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-system-domain --directory=workspace/system/domain --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-system-meta --directory=workspace/system/meta --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-system-meta-tests --directory=workspace/system/meta-tests --bundler=none --unitTestRunner=jest --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-system-meta-json --directory=workspace/system/meta-json --bundler=none --unitTestRunner=none --minimal=true --strict=false
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=workspace-system-meta-json-tests --directory=workspace/system/meta-json-test --bundler=none --unitTestRunner=jest --minimal=true --strict=false

# Database
# System
npx nx g @nx/js:lib --projectNameAndRootFormat=as-provided --name=database-system-protocol-json --directory=database/system/protocol-json --bundler=none --unitTestRunner=none --minimal=true --strict=false
