build_racustommenu() {
    project=$1
    echo "Building project: RaCustomMenu"
    cd "RaCustomMenu" || { echo "Failed to enter RaCustomMenu"; exit 1; }

    if ! (dotnet build -v quiet -o ../compile/ > /dev/null 2>&1); then
        echo -e "\033[31mBuild failed for RaCustomMenu\033[0m"
        exit 1
    else
        echo -e "\033[32mBuild succeeded for RaCustomMenu\033[0m"
    fi

    cp -f ../compile/RaCustomMenu.dll ../../TestPlugin/server/Plugins/
    # cp -f ../compile/TestPlugin.dll ../releases/

    cd ..
}

projects=(
          
)

max_jobs=$(nproc) # or set manually, e.g., 4
current_jobs=1

build_racustommenu &

build_project() {
    project=$1
    echo "Building project: $project"
    cd "$project" || { echo "Failed to enter $project"; exit 1; }

    if ! (dotnet build -v quiet -o ../compile/ > /dev/null 2>&1); then
        echo -e "\033[31mBuild failed for $project\033[0m"
        exit 1
    else
        echo -e "\033[32mBuild succeeded for $project\033[0m"
    fi

    cp -f ../compile/"$project".dll ../../TestPlugin/server/Configs/CedMod/CedModEvents/
    cp -f ../compile/"$project".dll ../releases/events/
    rm -rf ./release/

    cd ..
}

# for project in "${projects[@]}"; do
#     build_project "$project" &
#     ((current_jobs++))

#     # Wait if we've reached max parallel jobs
#     if (( current_jobs >= max_jobs )); then
#         wait -n  # wait for any job to finish
#         ((current_jobs--))
#     fi
# done

# Wait for all background jobs to finish
wait

# zip -9q ./releases/AllEvents releases/events/*

echo -e "\033[0mDone. All files stored in ./releases/"
