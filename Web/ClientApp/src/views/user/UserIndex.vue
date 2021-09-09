<template>
  <v-col
    cols="12"
    md="9"
  >
    <v-card flat>
      <v-card-title>Members list</v-card-title>
      <v-card-subtitle>All Member of Dr. Forums</v-card-subtitle>

      <v-card-text class="pt-0">
        <v-row>
          <v-col cols="12" md="6" lg="3">
            <v-text-field
              v-debounce="(x) => query.search = x"
              :value="query.search"
              dense
              append-icon="search"
              label="Search"
              single-line
              hide-details
            >
            </v-text-field>
          </v-col>
          <v-col cols="6" md="3" lg="2" xl="2">
            <auto-complete-select
              label="City"
              uri="regions/cities/all"
              item-text="name"
              v-model="query.city"
              item-value="slug"
              dense
              single-line
              hide-details
            >
            </auto-complete-select>
          </v-col>
          <v-col cols="6" md="3" lg="2" xl="2">
            <auto-complete-select
              label="Country"
              uri="regions/countries/all"
              item-text="name"
              v-model="query.country"
              item-value="slug"
              dense
              single-line
              hide-details
            >
            </auto-complete-select>
          </v-col>
          <v-col cols="6" md="3" lg="2">
            <auto-complete-select
              label="Speciality"
              uri="specialties/all"
              item-text="name"
              v-model="query.speciality"
              item-value="slug"
              dense
              single-line
              hide-details
            >
            </auto-complete-select>
          </v-col>

          <v-col cols="6" md="3" lg="2">
            <auto-complete-select
              label="Work Position"
              uri="exp/positions/all"
              item-text="name"
              v-model="query.position"
              item-value="slug"
              dense
              single-line
              hide-details
            >
            </auto-complete-select>
          </v-col>

          <v-col cols="6" md="3" lg="1">
            <user-sort-select
              class="mb-2"
              style="max-width: 200px"
              v-model="query.sort"
              dense
              single-line
              hide-details
            ></user-sort-select>
          </v-col>
        </v-row>
        <v-skeleton-loader v-if="meta.loading" type="list-item-avatar-three-line@4">
        </v-skeleton-loader>
        <div v-else-if="!data.length">
          <div class="body-1 text-center pa-8">Empty :(</div>
        </div>
        <template v-else>
          <div v-for="user in data" :key="user.uid">
            <user-row :user="user"></user-row>
            <v-divider class="mb-3"></v-divider>
          </div>
        </template>

        <v-pagination
          v-model="query.page"
          :total-visible="$vuetify.breakpoint.mdAndUp ? 8 : 4"
          :length="meta.totalPages"
        ></v-pagination>
      </v-card-text>
    </v-card>
  </v-col>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import ForumList from '@/components/forum/ForumList.vue';
import { useQuery } from '@/services/http';
import { Dictionary } from '@/services/model';
import { useBreadcrumbs } from '@/composable/breadcrumbs';
import ForumSortSelect from '@/components/form/ForumSortSelect.vue';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';
import UserRow from '@/components/user/UserRow.vue';
import UserSortSelect from '@/components/form/UserSortSelect.vue';

export default defineComponent({
  name: 'UserIndex',
  components: { UserSortSelect, UserRow, AutoCompleteSelect, ForumSortSelect, ForumList },
  setup() {
    useTitle('Forums Members');
    useBreadcrumbs([
      { text: 'Home', name: 'Home' },
      { text: 'Users', disabled: true },
    ]);

    const { query, data, meta } = useQuery<Dictionary>('users/infos')({
      search: '',
      speciality: [],
      city: '',
      country: '',
      position: '',
      sort: '',
    });

    return {
      query,
      data,
      meta,
    };
  },
});
</script>
