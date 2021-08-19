<template>
  <v-card class="elevation-2">
    <v-card-title>
      <v-text-field
        v-debounce="x => query.search = x"
        :value="query.search"
        append-icon="search"
        label="Search"
        single-line
        hide-details
      >
      </v-text-field>
      <v-spacer></v-spacer>
      <v-btn color="primary">Create</v-btn>
    </v-card-title>
    <v-data-table
      :headers="headers"
      :items="data"
      :server-items-length="meta.totalItems"
      :items-per-page="query.size"
      :page="query.page"
      :loading="meta.loading"
    >
      <template #item.icon="{ item }">
        <v-icon>{{ item.icon || 'folder' }}</v-icon>
      </template>

      <template #item.createdAt="{ item }">
        {{ formatDate(item.createdAt) }}
      </template>

      <template #item.updatedAt="{ item }">
        {{ formatDate(item.updatedAt) }}
      </template>

      <template #item.action="{ item }">
        <v-btn
          class="mx-2"
          icon
          color="primary"
        >
          <v-icon dark>
            edit
          </v-icon>
        </v-btn>

        <v-btn
          class="mx-2"
          icon
          color="error"
          @click="remove(item.slug)"
        >
          <v-icon dark>
            delete
          </v-icon>
        </v-btn>
      </template>
    </v-data-table>
  </v-card>
</template>

<script lang="ts">
import { useTitle } from '@vueuse/core';
import { defineComponent } from '@vue/composition-api';
import { useHttp, useQuery } from '@/services/http';
import { formatDate } from '@/composable/date';
import { Category } from '@/services/model';

export default defineComponent({
  name: 'ManageCategory',
  setup() {
    useTitle('Manage Category');
    const headers = [
      {
        text: 'Id',
        sortable: false,
        value: 'slug',
      },
      { text: 'icon', value: 'icon' },
      { text: 'Name', value: 'name' },
      { text: 'Created At', value: 'createdAt' },
      { text: 'Action', value: 'action' },
    ];

    const {
      query,
      data,
      meta,
    } = useQuery<Category, { search: string }>('/categories');
    const http = useHttp();

    async function remove(slug: string) {
      await http.delete(`/categories/${slug}`);
      data.value = data.value.filter((item) => item.slug !== slug);
    }

    return {
      query,
      meta,
      data,
      headers,
      formatDate,
      remove,
    };
  },
});
</script>
