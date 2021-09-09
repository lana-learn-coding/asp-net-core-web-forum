<template>
  <v-row class="py-2">
    <v-col cols="2" xl="1" class="d-flex flex-column align-center justify-center">
      <v-avatar>
        <v-img
          :src="user.avatar || require('@/assets/anon.png')"
          lazy-src="@/assets/anon_thumbnail.png"
          :alt="user.username"
        ></v-img>
      </v-avatar>
    </v-col>
    <v-col cols="10" md="6" class="d-flex flex-column">
      <router-link
        :to="{ name: 'Profile', params: {  slug: user.slug } }"
        class="font-weight-medium text-link text-decoration-none"
      >
        {{ user.username }}
      </router-link>
      <div>Join date: {{ formatDate(user.createdAt) }}</div>
      <div class="d-flex">
        <div class="d-flex align-center mr-2">
          <v-icon class="material-icons-outlined mr-1" small>forum</v-icon>
          <span>{{ user.threadsCount }}</span>
        </div>
        <v-icon class="material-icons-outlined mr-2" small v-if="user.showEmail">email</v-icon>
        <v-icon class="material-icons-outlined mr-2" small v-if="user.showPhone">call</v-icon>
      </div>
    </v-col>
    <v-col cols="10" md="4" class="d-flex flex-column flex-sm-row flex-md-column">
      <div class="mr-3">
        <v-icon small class="material-icons-outlined">place</v-icon>
        <span v-if="!user.workCountryId" class="ml-2">unknown</span>
        <span class="ml-2" v-else>
          {{ user.workCountry.name }}, {{ user.workCityId ? user.workCity.name : 'Unknown' }}
        </span>
      </div>
      <div>
        <v-icon small class="material-icons-outlined">business_center</v-icon>
        <div class="ml-2 d-inline">
          <span>
            {{ user.workPositionId ? user.workPosition.name : 'Unknown' }},
          </span>
          <span>{{ user.workExperienceId ? user.workExperience.measurement : 'Unknown' }}</span>
        </div>
      </div>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { defineComponent, PropType } from '@vue/composition-api';
import { formatDate } from '@/composable/date';
import { Me } from '@/services/model';
import NumberCounter from '@/components/NumberCounter.vue';

export default defineComponent({
  name: 'UserRow',
  components: { NumberCounter },
  props: {
    user: {
      required: true,
      type: Object as PropType<Me>,
    },
  },
  setup() {
    return {
      formatDate,
    };
  },
});
</script>
