<template>
  <v-container class="mt-2 mt-md-3 mt-lg-4 mt-xl-5">
    <app-breadcrumbs class="mb-2 mb-md-3 mb-lg-4"></app-breadcrumbs>
    <v-row>
      <v-col cols="12" md="3" v-if="$vuetify.breakpoint.smAndDown">
        <user-simple-card :user="user"></user-simple-card>
      </v-col>
      <v-col cols="12" md="9">
        <v-card flat>
          <v-skeleton-loader v-if="!user.uid" type="card-heading"></v-skeleton-loader>
          <v-card-title v-else>{{ user.username }}'s Profile</v-card-title>
          <v-card-text>
            <v-skeleton-loader v-if="!user.uid" type="paragraph@4"></v-skeleton-loader>
            <v-row v-else>
              <v-col cols="12" lg="10">
                <v-row>
                  <v-col cols="12" sm="6" lg="3">
                    <v-text-field
                      :value="user.firstName || 'Unknown'"
                      label="First Name"
                      dense
                      persistent-placeholder
                      readonly>
                    </v-text-field>
                  </v-col>
                  <v-col cols="12" sm="6" lg="3">
                    <v-text-field
                      :value="user.lastName || 'Unknown'"
                      dense
                      label="Last Name"
                      persistent-placeholder
                      readonly>
                    </v-text-field>
                  </v-col>
                  <v-col cols="12" sm="6" lg="3">
                    <v-text-field
                      :value="user.email || 'unknown'"
                      dense
                      label="Email"
                      persistent-placeholder
                      readonly
                    >
                    </v-text-field>
                  </v-col>
                  <v-col cols="12" sm="6" lg="3">
                    <v-text-field
                      :value="user.phone || 'unknown'"
                      dense
                      label="Phone Number"
                      persistent-placeholder
                      readonly
                    >
                    </v-text-field>
                  </v-col>
                  <v-col cols="12" sm="8" lg="5" xl="6">
                    <v-text-field
                      :value="user.dateOfBirth || 'unknown'"
                      dense
                      label="BirthDate"
                      persistent-placeholder
                      readonly
                    >
                    </v-text-field>
                  </v-col>
                  <v-col cols="12" sm="4" lg="3">
                    <gender-select
                      :value="user.gender"
                      dense
                      label="Gender"
                      persistent-placeholder
                      readonly
                    >
                    </gender-select>
                  </v-col>
                  <v-col cols="12" lg="8" xl="9">
                    <v-textarea
                      :value="user.bio || 'Nothing'"
                      dense
                      label="Bio"
                      persistent-placeholder
                      rows="4"
                      outlined
                      readonly
                    >
                    </v-textarea>
                  </v-col>
                </v-row>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
        <v-card class="mt-4" flat>
          <v-skeleton-loader v-if="!user.uid" type="card-heading"></v-skeleton-loader>
          <v-card-title v-else>{{ user.username }}'s Experience</v-card-title>
          <v-card-text>
            <v-skeleton-loader v-if="!user.uid" type="paragraph@4"></v-skeleton-loader>
            <v-row v-else>
              <v-col cols="12" lg="10">
                <v-row>
                  <v-col cols="12" sm="6" lg="4">
                    <v-text-field
                      :value="user.workCountry ? user.workCountry.name : 'unknown'"
                      dense
                      label="Work Country"
                      persistent-placeholder
                      readonly
                    >
                    </v-text-field>
                  </v-col>
                  <v-col cols="12" sm="6" lg="5">
                    <v-text-field
                      :value="user.workCity ? user.workCity.name : 'unknown'"
                      dense
                      label="Work City"
                      persistent-placeholder
                      readonly
                    >
                    </v-text-field>
                  </v-col>
                  <v-col cols="12" lg="5" xl="6">
                    <v-text-field
                      :value="user.workAddress || 'unknown'"
                      label="Work address"
                      persistent-placeholder
                      readonly
                      dense
                    ></v-text-field>
                  </v-col>
                  <v-col cols="12" lg="4" xl="3">
                    <v-text-field
                      :value="user.workPhone || 'unknown'"
                      label="Work phone number"
                      persistent-placeholder
                      readonly
                      dense
                    ></v-text-field>
                  </v-col>
                  <v-col cols="12" sm="6" lg="4">
                    <v-text-field
                      :value="user.workPositionId ? user.workPosition.name : 'unknown'"
                      dense
                      label="Work Position"
                      persistent-placeholder
                      readonly
                    >
                    </v-text-field>
                  </v-col>
                  <v-col cols="12" sm="6" lg="5">
                    <v-text-field
                      :value="user.workExperienceId ? user.workExperience.measurement : 'unknown'"
                      dense
                      label="Work Experience"
                      persistent-placeholder
                      readonly
                    >
                    </v-text-field>
                  </v-col>
                  <v-col cols="12" lg="8" xl="9">
                    <auto-complete-select
                      uri="specialties/all"
                      v-model="user.workSpecialitiesIds"
                      label="I have experience in"
                      item-value="uid"
                      item-text="name"
                      persistent-placeholder
                      readonly
                      dense
                      multiple
                    >
                    </auto-complete-select>
                  </v-col>
                  <v-col cols="12" lg="8" xl="9">
                    <v-textarea
                      :value="user.workDescription || 'Nothing to say'"
                      dense
                      label="Work description"
                      persistent-placeholder
                      rows="3"
                      outlined
                      readonly
                    >
                    </v-textarea>
                  </v-col>
                </v-row>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
      <v-col cols="12" md="3">
        <user-simple-card v-if="$vuetify.breakpoint.mdAndUp" :user="user"
                          class="mb-4"></user-simple-card>
        <app-forum-statistics class="mb-4"></app-forum-statistics>
        <app-latest-threads></app-latest-threads>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { defineComponent, reactive, watch } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import TheLayout from '@/layouts/TheLayout.vue';
import AppBreadcrumbs from '@/components/app/AppBreadcrumbs.vue';
import AppForumStatistics from '@/components/app/AppForumStatistics.vue';
import AppUserNavigation from '@/components/app/AppUserNavigation.vue';
import { useHttp } from '@/services/http';
import { Me } from '@/services/model';
import UserSimpleCard from '@/components/user/UserSimpleCard.vue';
import AppLatestThreads from '@/components/app/AppLatestThreads.vue';
import GenderSelect from '@/components/form/GenderSelect.vue';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';
import { useBreadcrumbs } from '@/composable/breadcrumbs';

export default defineComponent({
  name: 'Profile',
  components: {
    AutoCompleteSelect,
    GenderSelect,
    AppLatestThreads,
    UserSimpleCard,
    AppUserNavigation,
    AppForumStatistics,
    AppBreadcrumbs,
    TheLayout,
  },
  props: {
    slug: String,
  },
  setup(props) {
    useTitle('User Profile');
    useBreadcrumbs([
      { text: 'Home', name: 'Home' },
      { text: 'Users', name: 'Users' },
      { text: 'Profile', disabled: true },
    ]);
    const user = reactive({ slug: '', uid: '', username: 'user' } as unknown as Me);
    const http = useHttp();

    watch(() => props.slug, async (val) => {
      const res = await http.get<Me>(`users/infos/${val}`);
      Object.assign(user, res);
      useTitle('User Profile');
      useBreadcrumbs([
        { text: 'Home', name: 'Home' },
        { text: 'Users', name: 'Users' },
        { text: res.username, disabled: true },
      ]);
    }, { immediate: true });

    return { user };
  },
});
</script>
