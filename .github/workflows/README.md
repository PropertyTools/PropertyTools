# Release Workflows

This directory contains GitHub Actions workflows for automating the release process of PropertyTools NuGet packages.

## Workflows

### release-preview.yml
Publishes preview NuGet packages when a preview tag is pushed.

**Trigger:** Tags matching the pattern `v[0-9]+.[0-9]+.[0-9]+-preview[0-9][0-9][0-9]`
- Example: `v3.2.0-preview001`

**Actions:**
1. Extracts release notes from the "Unreleased" section of CHANGELOG.md
2. Extracts version number from the tag
3. Builds the solution with MSBuild
4. Runs tests
5. Packs NuGet packages (PropertyTools, PropertyTools.Wpf, PropertyTools.Wpf.Extended.Toolkit)
6. Pushes packages to GitHub Packages
7. Creates a GitHub pre-release with the packages attached

### release.yml
Publishes stable release NuGet packages when a release tag is pushed.

**Trigger:** Tags matching the pattern `v[0-9]+.[0-9]+.[0-9]+` (excluding preview tags)
- Example: `v3.2.0`

**Actions:**
1. Verifies the commit exists in origin/main (safety check)
2. Extracts release notes from the version-specific section in CHANGELOG.md
3. Extracts version number from the tag
4. Builds the solution with MSBuild
5. Runs tests
6. Packs NuGet packages (PropertyTools, PropertyTools.Wpf, PropertyTools.Wpf.Extended.Toolkit)
7. Pushes packages to GitHub Packages
8. Pushes packages to nuget.org (requires NUGET_API_KEY secret)
9. Creates a GitHub release with the packages attached

## Security Considerations

### Secrets Management
- **GITHUB_TOKEN**: Automatically provided by GitHub Actions, scoped to the repository. Used for:
  - Pushing packages to GitHub Packages
  - Creating GitHub releases
- **NUGET_API_KEY**: Must be configured as a repository secret. Used for pushing packages to nuget.org.

### Security Best Practices Implemented
1. **Branch verification**: The release workflow verifies that the tagged commit exists in the main branch
2. **Token scoping**: Uses minimal permissions required for each operation
3. **Skip duplicates**: Uses `--skip-duplicate` flag to prevent accidental republishing
4. **Read-only checkout**: Repository is checked out read-only; workflows don't commit back
5. **Timeout limits**: Workflows have 30-minute timeout to prevent runaway jobs

### Potential Security Risks
1. **Secret exposure**: If NUGET_API_KEY is compromised, attackers could publish malicious packages
   - Mitigation: Use GitHub's encrypted secrets, rotate keys regularly, audit package publications
2. **Tag manipulation**: If repository access is compromised, malicious tags could trigger releases
   - Mitigation: Enable branch protection, require signed commits, use 2FA for maintainers
3. **Dependency confusion**: Malicious packages could be published with similar names
   - Mitigation: Package signing (see below), monitoring package downloads/usage

### Package Signing
The projects are currently configured with strong-name signing (AssemblyOriginatorKeyFile). For additional security:

1. **NuGet package signing**: Consider signing packages with a code signing certificate
   - Add `<PackageCertificate>` to .csproj files
   - Store certificate in GitHub secrets
   - Add signing step: `dotnet nuget sign Packages/*.nupkg --certificate-path <cert> --timestamper <url>`

2. **Benefits**:
   - Verifies package integrity
   - Prevents tampering
   - Builds trust with consumers

3. **Implementation**: Requires obtaining a code signing certificate from a trusted CA

## Usage

### Creating a Preview Release
```bash
git tag v3.2.0-preview001
git push origin v3.2.0-preview001
```

### Creating a Stable Release
```bash
# Ensure you're on the main branch
git checkout main
git tag v3.2.0
git push origin v3.2.0
```

### Prerequisites
1. Update CHANGELOG.md with release notes before tagging
2. Ensure all tests pass
3. For stable releases, create the tag from the main branch

### Required Secrets
Configure in repository Settings → Secrets and variables → Actions:
- `NUGET_API_KEY`: API key from nuget.org for publishing packages

## Troubleshooting

### Release failed - "Verify commit exists in origin/main"
The tagged commit must exist in the main branch. Cherry-pick or merge the commit to main before tagging.

### Release failed - "unauthorized" on NuGet push
Verify that NUGET_API_KEY secret is configured correctly and the API key hasn't expired.

### Packages not appearing on nuget.org
Check that:
1. The package version doesn't already exist
2. The NUGET_API_KEY has permissions to publish the package ID
3. Review the workflow logs for push errors
