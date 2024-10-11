<template>
    <v-card class="mt-2">
      <v-card-title>{{ section.Title }}</v-card-title>
      <v-card-text>{{ section.Description }}</v-card-text>
      <v-card-text>{{ section.UserDescription }}</v-card-text>
      <v-card-actions>
        <v-btn v-if="section.CanImport" @click="importSection(section.Id)" color="success">Import</v-btn>
        <v-btn v-if="section.CanDelete" @click="deleteSection(section.Id)" color="error">Delete</v-btn>
      </v-card-actions>
    </v-card>
  </template>
  
  <script setup lang="ts">
  defineProps<{
    section: {
      Id: number;
      Title: string;
      Description: string;
      UserDescription: string;
      CanImport: boolean;
      CanDelete: boolean;
    };
    importSection: (sectionId: number) => void;
    deleteSection: (sectionId: number) => void;
  }>();
  </script>
  
  <style scoped>
  /* Add any specific styles for the StoredSectionCard here */
  </style>
